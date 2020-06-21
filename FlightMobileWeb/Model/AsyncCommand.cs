using System.Threading.Tasks;

namespace FlightMobileWeb.Model
{
    public enum Result {Ok, NotOk, Timeout, Disconnected, ConnectionLost, FailUpdate }
    public class AsyncCommand
    {
        public Command Command { get; private set; }

        public Task<Result> Task { get => Completion.Task; }
        public TaskCompletionSource<Result> Completion { get; private set; }

        public AsyncCommand(Command cmd)
        {
            this.Command = cmd;
            Completion = new TaskCompletionSource<Result>
                (TaskCreationOptions.RunContinuationsAsynchronously);
        }
    }
}