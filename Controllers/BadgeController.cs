using GamifyBackEnd.DB;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace GamifyBackEnd.Controllers
{
    [ApiController]
    [Route("api/badge")]
    public class BadgeController : Controller
    {
        // GET: api/badge/user/{user_name}
        [HttpGet("user/{user_name}")]
        public IActionResult GetbadgesForUser(string user_name)
        {
            try
            {
                using (var db = new GameDbContext())
                {
                    var user = db.Users.FirstOrDefault(u => u.Name == user_name);
                    if (user == null)
                        return NotFound("User not found");

                    var badgeIds = db.badgeusers
                        .Where(bu => bu.user_id == user.Id)
                        .Select(bu => bu.badge_id)
                        .ToList();

                    var badges = db.badges
                        .Where(b => badgeIds.Contains(b.Id))
                        .ToList();

                    return Ok(badges);
                }
            }
            catch (Exception e)
            {
                return BadRequest("Database failure: " + e.Message);
            }
        }

        // POST: api/badge/give
        [HttpPost("give")]
        public IActionResult GiveBadgeToUser([FromBody] BadgeAssignmentDto dto)
        {
            try
            {
                using (var db = new GameDbContext())
                {
                    var user = db.Users.FirstOrDefault(u => u.Name == dto.user_name);
                    var badge = db.badges.FirstOrDefault(b => b.name == dto.badge_name);

                    if (user == null || badge == null)
                        return NotFound("User or Badge not found");

                    bool alreadyHas = db.badgeusers
                        .Any(bu => bu.user_id == user.Id && bu.badge_id == badge.Id);

                    if (alreadyHas)
                        return Conflict("User already has this badge");

                    var badgeUser = new BadgeUser
                    {
                        user_id = user.Id,
                        badge_id = badge.Id
                    };

                    db.badgeusers.Add(badgeUser);
                    db.SaveChanges();

                    return Ok("Badge assigned successfully");
                }
            }
            catch (Exception e)
            {
                return BadRequest("Database failure: " + e.Message);
            }
        }

        // POST: api/badge/add
        [HttpPost("add")]
        public async Task<IActionResult> AddBadge([FromForm] string name, [FromForm] IFormFile image, [FromForm] IFormFile imageempty)
        {
            if (image == null || imageempty == null)
                return BadRequest("Images are required.");

            try
            {
                
                using var ms1 = new MemoryStream();
                await image.CopyToAsync(ms1);
                byte[] imageBytes = ms1.ToArray();

                using var ms2 = new MemoryStream();
                await imageempty.CopyToAsync(ms2);
                byte[] imageEmptyBytes = ms2.ToArray();

                var badge = new Badge
                {
                    name = name,
                    image = imageBytes,
                    imageempty = imageEmptyBytes
                };


                using var db = new GameDbContext();
                db.badges.Add(badge);
                Console.WriteLine("I added");
                await db.SaveChangesAsync();

                Console.WriteLine("I uploaded");

                return Ok("Badge created successfully");
            }

            catch (Exception e)
            {

                if (e.InnerException != null)
                {
                    Console.WriteLine("Inner error: " + e.InnerException.Message);
                }
                return BadRequest("Database failure: " + e.Message);


            }
        }

        // DELETE: api/badge/delete/{id}
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteBadge(int id)
        {
            try
            {
                using (var db = new GameDbContext())
                {
                    var badge = db.badges.FirstOrDefault(b => b.Id == id);
                    if (badge == null)
                        return NotFound("Badge not found");

                    var related = db.badgeusers.Where(bu => bu.badge_id == id);
                    db.badgeusers.RemoveRange(related);
                    db.badges.Remove(badge);

                    db.SaveChanges();
                    return Ok("Badge deleted");
                }
            }
            catch (Exception e)
            {
                return BadRequest("Database failure: " + e.Message);
            }
        }


        [HttpGet("halloffame/{userName}")]
        public IActionResult GetBadgeShowcase(string userName)
        {

            try
            {
                using var db = new GameDbContext();

                var user = db.Users
                    .FirstOrDefault(u => u.Name.ToLower() == userName.ToLower());

                // No user found? Still show all badges with imageempty
                HashSet<int> userBadgeIds = new();
                if (user != null)
                {
                    userBadgeIds = db.badgeusers
                        .Where(bu => bu.user_id == user.Id)
                        .Select(bu => bu.badge_id)
                        .ToHashSet();
                }

                var badges = db.badges
                    .ToList() // Force eval before converting
                    .Select(b => new
                    {
                        Name = b.name,
                        Image = userBadgeIds.Contains(b.Id)
                            ? Convert.ToBase64String(b.image)
                            : Convert.ToBase64String(b.imageempty)
                    })
                    .ToList();

                return Ok(badges);
            }
            catch (Exception ex)
            {
                Console.WriteLine("HALL OF FAME ERROR:");
                Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
                return BadRequest("Database failure: " + ex.Message);
            }


        }



    }

    public class BadgeAssignmentDto
    {
        public string user_name { get; set; }
        public string badge_name { get; set; }
    }
}
