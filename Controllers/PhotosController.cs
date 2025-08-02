using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // Potentially unused, but harmless
using Microsoft.EntityFrameworkCore;
using SKLadiesCorner.Data;
using SKLadiesCorner.Models;
using Microsoft.AspNetCore.Http; // Required for IFormFile
using System.IO;                  // Required for Path, File operations
// Removed: using Microsoft.AspNetCore.Http.HttpResults; // This caused a compile error

namespace SKLadiesCorner.Controllers
{
    public class PhotosController : Controller
    {
        // Ensure this matches your actual DbContext class name
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PhotosController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // --- Helper Methods for Photo Handling ---

        // Method to save the image file to the server and return its relative path
        private async Task<string?> SavePhotoFile(IFormFile? file)
        {
            // SYNTAX FIX: Added missing opening parenthesis
            if (file == null || file.Length == 0)
            {
                return null; // No file uploaded or empty file
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            // FIX: Return null immediately if validation fails
            if (!allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("ImageFile", "Only JPG, JPEG, PNG, and GIF images are allowed.");
                return null; // Indicate validation failure
            }

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Using '-' instead of '_' as a separator, which is fine.
            string uniqueFileName = Guid.NewGuid().ToString() + "-" + Path.GetFileName(file.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // LOGIC FIX: Return uniqueFileName, not filePath
            return "/images/products/" + uniqueFileName;
        }

        // Method to delete the image file from the server
        private void DeletePhotoFile(string? fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                // Construct the full path to the file
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products", fileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }

        // --- END Helper Methods ---


        // GET: Photos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Photo.ToListAsync());
        }

        // GET: Photos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // GET: Photos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Photos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        // BIND FIX: Only bind 'Name' and 'ImageFile' for Create
        public async Task<IActionResult> Create(
            [Bind("Name,ImageFile,Category,Color,Size,IsActive,Description,Price,SKU,StockQuantity,CreatedAt,UpdatedAt")] Photo photo)
        {
            // The check for photo.ImageFile == null can be done here explicitly if it's required.
            // if (photo.ImageFile == null)
            // {
            //     ModelState.AddModelError("ImageFile", "An image file is required for creation.");
            // }

            if (ModelState.IsValid)
            {
                try
                {
                    string? uniqueFileName = await SavePhotoFile(photo.ImageFile);
                    // This check handles both 'file == null' case and validation failures within SavePhotoFile
                    if (uniqueFileName == null && photo.ImageFile != null)
                    {
                        // If uniqueFileName is null but ImageFile was provided,
                        // it means validation in SavePhotoFile failed and error was added to ModelState.
                        return View(photo);
                    }
                    // If photo.ImageFile was null from the start, uniqueFileName will be null, and it's okay not to assign it.
                    photo.ImagePath = uniqueFileName; // Store the file name in the database field

                    _context.Add(photo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving to the database: " + ex.Message);
                    // Consider logging ex.InnerException.Message for more details
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unexpected error occurred: " + ex.Message);
                }
            }
            return View(photo); // Return to view if ModelState is not valid or an exception occurred
        }

        // GET: Photos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photo.FindAsync(id);
            if (photo == null)
            {
                return NotFound();
            }
            return View(photo);
        }

        // POST: Photos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        // BIND FIX: Ensure ImageFile is bound for uploads, ImagePath is not.
        public async Task<IActionResult> Edit(int id, 
            [Bind("Id,Name,ImageFile,Category,Color,Size,IsActive,Description,Price,SKU,StockQuantity,CreatedAt,UpdatedAt")] Photo photo)
        {
            if (id != photo.Id)
            {
                return NotFound();
            }

            // Fetch the existing entity from the database explicitly.
            var existingPhoto = await _context.Photo.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (existingPhoto == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string? currentImagePathInDb = existingPhoto.ImagePath; // Get the existing path from DB

                    if (photo.ImageFile != null) // A new file has been uploaded
                    {
                        // LOGIC FIX: Delete the old photo file from the server
                        DeletePhotoFile(currentImagePathInDb);

                        // Save the new photo file
                        string? newUniqueFileName = await SavePhotoFile(photo.ImageFile);
                        if (newUniqueFileName == null) // Failed to save due to validation (e.g., wrong type)
                        {
                            // If file save failed, ensure the view gets the existing path back
                            photo.ImagePath = currentImagePathInDb; // Preserve existing path for display in error view
                            return View(photo); // Return to view with validation errors
                        }
                        photo.ImagePath = newUniqueFileName; // Update the model with the new path
                    }
                    else
                    {
                        // No new file uploaded, retain the old photo path
                        photo.ImagePath = currentImagePathInDb;
                    }

                    _context.Update(photo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhotoExists(photo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // Re-throw if it's a true concurrency error
                    }
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving updates to the database: " + ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unexpected error occurred: " + ex.Message);
                }
            }
            // If ModelState is not valid, or an exception occurred, return to the view.
            // Ensure the ImagePath is preserved if coming back from an error.
            if (photo.ImagePath == null)
            {
                // This fetches the path from the DB again if it somehow got lost,
                // useful if coming back to the view due to other validation errors.
                var currentDbPhoto = await _context.Photo.AsNoTracking().FirstOrDefaultAsync(m => m.Id == photo.Id);
                photo.ImagePath = currentDbPhoto?.ImagePath;
            }

            return View(photo);
        }

        // GET: Photos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var photo = await _context.Photo.FindAsync(id);
            if (photo != null)
            {
                // Delete the physical file from wwwroot/images folder
                DeletePhotoFile(photo.ImagePath);

                _context.Photo.Remove(photo);
                // FIX: Only save changes if photo was found and removed
                await _context.SaveChangesAsync();
            }
            // If photo was null, it's already "deleted" or never existed, so still redirect
            return RedirectToAction(nameof(Index));
        }

        private bool PhotoExists(int id)
        {
            return _context.Photo.Any(e => e.Id == id);
        }
    }
}