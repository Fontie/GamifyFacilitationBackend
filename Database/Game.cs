
namespace GamifyBackEnd.DB
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] ZipData { get; set; } // Stores the Unity game as a .zip file. end result should be a BLOB in the db.
        public string LevelName { get; set; }
    }
}