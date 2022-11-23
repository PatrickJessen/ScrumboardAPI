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
        public List<BoardState> States { get; set; }
        public Board()
        {
            States = new List<BoardState>();
        }
    }
}
