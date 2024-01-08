using Configuration.Domain.Applications;
using Xunit;

namespace Configuration.Domain.Tests.Domain.Models.Entities;

public class ApplicationTests : IDisposable
{
    // why xUnit over MSTest or nUnit? - https://app.pluralsight.com/course-player?clipId=301b24cf-5f4c-4868-915f-2a268efb4033

    // the template here is {1}_{2}_{3}
    // 1: a name for the unit that is being tested (CreateEmployee)
    // 2: the scenario under which the unit is being tested (ConstructInternalEmployee)
    // 3: the expected behavior when the scenario is invoked (SalaryMustBe2500)

    public ApplicationTests()
    {
        // this constructor is called for every test (context is not shared)
    }

    public void Dispose()
    {
        // clean up the setup code, if required
    }

    [Fact]
    [Trait("Category", "Application_New_Id")]
    [Trait("Category", "Application_New_StateAssignment_Default")]
    public void New_ConstructNewApplication_IdDefaultsToNewGuidWhenEmpty()
    {
        // arrange
        Guid id = Guid.NewGuid();
        string name = "Test Application";
        string? abbreviatedName = null;
        string? description = null;
        bool? isActive = null;

        // act
        Application application = Application.New(id, name, abbreviatedName, description, isActive);

        // assert
        Assert.True(application.Id.Value != Guid.Empty);
    }

    [Fact]
    [Trait("Category", "Application_New_Id")]
    [Trait("Category", "Application_New_StateAssignment_Default")]
    public void New_ConstructNewApplication_IdDefaultsToNewGuidWhenIdIsNull()
    {
        // arrange
        Guid? id = null;
        string name = "Test Application";
        string? abbreviatedName = null;
        string? description = null;
        bool? isActive = null;

        // act
        Application application = Application.New(id, name, abbreviatedName, description, isActive);

        // assert
        Assert.True(application.Id.Value != id);
        Assert.True(application.Id.Value != Guid.Empty);
    }

    [Fact]
    [Trait("Category", "Application_New_IsActive")]
    [Trait("Category", "Application_New_StateAssignment_Default")]
    public void New_ConstructNewApplication_IsActiveDefaultsToFalseWhenNotProvided()
    {
        // arrange
        Guid? id = null;
        string name = "Test Application";
        string? abbreviatedName = null;
        string? description = null;
        bool? isActive = null;

        // act
        Application application = Application.New(id, name, abbreviatedName, description, isActive);

        // assert
        Assert.True(application.IsActive == false);
    }

    [Fact]
    [Trait("Category", "Application_New_IsActive")]
    [Trait("Category", "Application_New_StateAssignment")]
    public void New_ConstructNewApplication_IsActiveIsRetainedWhenProvided()
    {
        // arrange
        Guid? id = null;
        string name = "Test Application";
        string? abbreviatedName = null;
        string? description = null;
        bool? isActive = true;

        // act
        Application application = Application.New(id, name, abbreviatedName, description, isActive);

        // assert
        Assert.True(application.IsActive);
    }
}