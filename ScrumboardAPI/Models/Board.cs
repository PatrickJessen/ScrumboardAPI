namespace ScrumboardAPI.Models
{
    public class BoardState
    {
        public string Title { get; set; }
        public TaskState State { get; set; }
        public List<Task> Tasks { get; set; }

        public BoardState(string title)
        {
            this.Title = title;
            Tasks = new List<Task>();
        }
    }
    public class Board
    {
        int Id { get; set; }
        public string Title { get; set; }
        public List<Task> Tasks { get; set; }
        public Board(int id, string title)
        {
            Id = id;
            Title = title;
            Tasks = new List<Task>();
        }
    }
}
