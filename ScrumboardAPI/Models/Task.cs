using System.Threading.Tasks;

namespace ScrumboardAPI.Models
{
    public enum TaskState { TO_DO, IN_PROGRESS, REVIEW, DONE }

    public enum TaskPriority { VERY_LOW, LOW, MEDIUM, HIGH, VERY_HIGH }
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Points { get; set; }
        public string AssignedTo { get; set; }
        public TaskState State { get; set; }
        public TaskPriority Priority { get; set; }

        public Task(string title, TaskState state, string description, int points, string assignedTo, TaskPriority priority)
        {
            this.Title = title;
            this.State = state;
            this.Description = description;
            this.Points = points;
            this.AssignedTo = assignedTo;
            this.Priority = priority;
        }

        public Task()
        {

        }
    }
}
