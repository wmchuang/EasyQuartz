using System;
using System.Threading;
using System.Threading.Tasks;
using Quartz;

namespace EasyQuartz.Listener;

public class CustomTriggerListener : ITriggerListener
{
    public string Name => "CustomTriggerListener";

    public async Task TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode, CancellationToken cancellationToken = default)
    {
        await Task.Run(() =>
        { 
            Console.WriteLine("this is TriggerComplete");
        });
    }

    public async Task TriggerFired(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        await Task.Run(() =>
        {
            Console.WriteLine("this is TriggerFired");
        });
    }

    public async Task TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken = default)
    {
        await Task.Run(() =>
        {
            Console.WriteLine("this is TriggerComplete");
        });
    }

    public async Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        await Task.Run(() =>
        {
            Console.WriteLine("VetoJobExecution");
        });
        return false;  //返回false才能继续执行
    }
}
