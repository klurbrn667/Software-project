using System;
using System.Linq;
using ThreadingTask = System.Threading.Tasks.Task;

using ConsoleAppSoftware;

class Program
{
    static void Main()
    {
        using (var context = new TaskContext())
        {
            context.Database.EnsureCreated();

            while (true)
            {
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1. Добавить задачу");
                Console.WriteLine("2. Показать задачи");
                Console.WriteLine("3. Завершить задачу");
                Console.WriteLine("0. Выход");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddTask(context);
                        break;
                    case "2":
                        ShowTasks(context);
                        break;
                    case "3":
                        CompleteTask(context);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Некорректный выбор. Попробуйте снова.");
                        break;
                }
            }
        }
    }

    static void AddTask(TaskContext context)
    {
        Console.Write("Введите описание задачи: ");
        var description = Console.ReadLine();

        Console.Write("Введите приоритет (целое число): ");
        int priority;
        while (!int.TryParse(Console.ReadLine(), out priority))
        {
            Console.WriteLine("Некорректный ввод. Попробуйте снова.");
            Console.Write("Введите приоритет (целое число): ");
        }

        var newTask = new ConsoleAppSoftware.Task
        {
            Description = description,
            IsCompleted = false,
            Priority = priority
        };


        context.Tasks.Add(newTask);
        context.SaveChanges();
        Console.WriteLine("Задача добавлена.");
    }

    static void ShowTasks(TaskContext context)
    {
        var tasks = context.Tasks.OrderBy(t => t.Priority).ToList();

        if (tasks.Count == 0)
        {
            Console.WriteLine("Нет задач.");
        }
        else
        {
            Console.WriteLine("Список задач:");
            foreach (var task in tasks)
            {
                Console.WriteLine($"[{task.Id}] {task.Description} (Приоритет: {task.Priority}, Завершена: {task.IsCompleted})");
            }
        }
    }

    static void CompleteTask(TaskContext context)
    {
        Console.Write("Введите ID задачи, которую вы хотите завершить: ");
        if (int.TryParse(Console.ReadLine(), out int taskId))
        {
            var task = context.Tasks.Find(taskId);

            if (task != null)
            {
                task.IsCompleted = true;
                context.SaveChanges();
                Console.WriteLine($"Задача с ID {taskId} завершена.");
            }
            else
            {
                Console.WriteLine($"Задача с ID {taskId} не найдена.");
            }
        }
        else
        {
            Console.WriteLine("Некорректный ввод. Пожалуйста, введите корректный ID задачи.");
        }
    }
}
