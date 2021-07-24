using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommandManager : Singleton<CommandManager>
{
    public bool DebugMode = false;
    public bool AutoBreak = true;
    private List<ICommand> queue = new List<ICommand>();

    public void QueueCommand(ICommand command)
    {
        queue.Add(command);
    }

    void Update()
    {
        if (!DebugMode)
        {
            if (AutoBreak)
            {
                foreach (var item in Enumerable.Range(0, 1000))
                {
                    if (queue.Any())
                    {
                        ExecuteNextCommand();
                    }
                }
            }
            else
            {
                while (queue.Any())
                {
                    ExecuteNextCommand();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            ExecuteNextCommand();
        }
    }

    void ExecuteNextCommand()
    {
        ICommand command = queue.First();
        queue.Remove(command);
        Debug.Log("Running command: " + command.Description);
        command.Execute();
    }
}

public interface ICommand
{
    string Description { get; }
    void Execute();
}

public class Command : ICommand
{
    private string description;
    public string Description => description;

    private Action execute;

    public void Execute()
    {
        this.execute();
    }

    public Command(string description, Action execute)
    {
        this.description = description;
        this.execute = execute;
    }
}