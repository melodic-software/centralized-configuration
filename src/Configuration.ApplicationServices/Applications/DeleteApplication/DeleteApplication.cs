using Enterprise.ApplicationServices.Commands.Model;

namespace Configuration.ApplicationServices.Applications.DeleteApplication;

public class DeleteApplication : ICommand
{
    public Guid Id { get; }

    public DeleteApplication(Guid id)
    {
        Id = id;
    }
}