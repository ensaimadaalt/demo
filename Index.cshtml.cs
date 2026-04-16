using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

public class IndexModel : PageModel
{
    public static List<TaskItem> Tasks = new List<TaskItem>();
    public List<TaskItem> FilteredTasks = new List<TaskItem>();

    [BindProperty]
    public string NewTask { get; set; }

    public void OnGet(string filter, int? toggle, int? delete)
    {
        if (toggle.HasValue)
        {
            var task = Tasks.FirstOrDefault(t => t.Id == toggle);
            if (task != null) task.IsCompleted = !task.IsCompleted;
        }

        if (delete.HasValue)
        {
            Tasks.RemoveAll(t => t.Id == delete);
        }

        FilterTasks(filter);
    }

    public IActionResult OnPost()
    {
        if (!string.IsNullOrEmpty(NewTask))
        {
            Tasks.Add(new TaskItem
            {
                Id = Tasks.Count + 1,
                Name = NewTask
            });
        }

        return RedirectToPage();
    }

    private void FilterTasks(string filter)
    {
        FilteredTasks = filter switch
        {
            "active" => Tasks.Where(t => !t.IsCompleted).ToList(),
            "completed" => Tasks.Where(t => t.IsCompleted).ToList(),
            _ => Tasks
        };
    }

    public class TaskItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
    }
}