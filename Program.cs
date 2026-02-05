using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public bool IsDone { get; set; }
    public DateTime CreatedAt { get; set; }
}

class Program
{
    static List<TaskItem> tasks = new();
    static string filePath = "tasks.json";

    static void Main()
    {
        LoadTasks();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Task Manager ===");
            Console.WriteLine("1. 작업 등록");
            Console.WriteLine("2. 작업 목록 조회");
            Console.WriteLine("3. 작업 완료 처리");
            Console.WriteLine("4. 종료");
            Console.Write("선택: ");

            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    AddTask();
                    break;
                case "2":
                    ShowTasks();
                    break;
                case "3":
                    CompleteTask();
                    break;
                case "4":
                    SaveTasks();
                    return;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Pause();
                    break;
            }
        }
    }

    static void AddTask()
    {
        Console.Write("작업 제목: ");
        string? title = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("제목은 필수입니다.");
            Pause();
            return;
        }

        int newId = tasks.Count == 0 ? 1 : tasks[^1].Id + 1;

        tasks.Add(new TaskItem
        {
            Id = newId,
            Title = title,
            IsDone = false,
            CreatedAt = DateTime.Now
        });

        SaveTasks();
        Console.WriteLine("작업이 등록되었습니다.");
        Pause();
    }

    static void ShowTasks()
    {
        Console.WriteLine("\n--- 작업 목록 ---");

        if (tasks.Count == 0)
        {
            Console.WriteLine("등록된 작업이 없습니다.");
        }
        else
        {
            foreach (var task in tasks)
            {
                string status = task.IsDone ? "[완료]" : "[미완료]";
                Console.WriteLine($"{task.Id}. {status} {task.Title}");
            }
        }

        Pause();
    }

    static void CompleteTask()
    {
        Console.Write("완료할 작업 ID 입력: ");
        string? input = Console.ReadLine();

        if (!int.TryParse(input, out int id))
        {
            Console.WriteLine("숫자를 입력하세요.");
            Pause();
            return;
        }

        var task = tasks.Find(t => t.Id == id);

        if (task == null)
        {
            Console.WriteLine("해당 ID의 작업이 없습니다.");
        }
        else
        {
            task.IsDone = true;
            SaveTasks();
            Console.WriteLine("작업이 완료되었습니다.");
        }

        Pause();
    }

    static void LoadTasks()
    {
        if (!File.Exists(filePath))
            return;

        string json = File.ReadAllText(filePath);
        tasks = JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new();
    }

    static void SaveTasks()
    {
        string json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(filePath, json);
    }

    static void Pause()
    {
        Console.WriteLine("\n엔터 키를 누르세요...");
        Console.ReadLine();
    }
}
