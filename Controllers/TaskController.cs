
using TaskApi.Models;
// using System.Collections.Generic;
// using System.Linq;
// using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace TaskApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    // private static List<TaskItem> taskItems = new List<TaskItem>();
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "tasks.json");


    [HttpGet]
    public IActionResult GetAllTasks()
    {
        var tasks = LoadTasks();
        return Ok(tasks);
    }


   [HttpPost]
    public IActionResult CreateTask([FromBody] TaskItem task)
    {
        var tasks = LoadTasks();
        task.Id = tasks.Any() ? tasks.Max(t => t.Id) + 1 : 1;
        tasks.Add(task);
        SaveTasks(tasks);
        return Ok(task);
    }


    [HttpPut("{id}")]
    public IActionResult UpdateTask(int id, [FromBody] TaskItem updated)
    {
        var tasks = LoadTasks();
        var existing = tasks.FirstOrDefault(t => t.Id == id);
        if (existing == null) return NotFound();

        existing.Title = updated.Title;
        existing.Description = updated.Description;
        existing.Priority = updated.Priority;
        existing.DueDate = updated.DueDate;
        existing.Status = updated.Status;

        SaveTasks(tasks);
        return Ok(existing);
    }



    [HttpDelete("{id}")]
    public IActionResult DeleteTask(int id)
    {
        var tasks = LoadTasks();
        var existing = tasks.FirstOrDefault(t => t.Id == id);
        if (existing == null) return NotFound();

        tasks.Remove(existing);
        SaveTasks(tasks);
        return NoContent();
    }


// Helper methods to load and save tasks
private List<TaskItem> LoadTasks()
    {
        if (!System.IO.File.Exists(_filePath))
            return new List<TaskItem>();

        var json = System.IO.File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
    }

    private void SaveTasks(List<TaskItem> tasks)
    {
        var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
        System.IO.File.WriteAllText(_filePath, json);
    }
}

   