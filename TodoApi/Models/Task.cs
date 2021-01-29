using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class Task
    {
        public long Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Date { get; set; }
        public bool IsComplete { get; set; } = false;
        public string Secret { get; set; }
    }
    
    public class TaskDto
    {
        public long Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Date { get; set; }
        public bool IsComplete { get; set; }
    }
}