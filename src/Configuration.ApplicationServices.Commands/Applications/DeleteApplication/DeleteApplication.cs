using Enterprise.ApplicationServices.Commands.Model;

namespace Configuration.ApplicationServices.Commands.Applications.DeleteApplication;

public class DeleteApplication : ICommand
{
    public Guid Id { get; }

    public DeleteApplication(Guid id)
    {
        Id = id;
    }
}