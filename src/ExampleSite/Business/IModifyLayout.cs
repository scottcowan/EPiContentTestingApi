using ExampleSite.Models.ViewModels;

namespace ExampleSite.Business
{
    /// <summary>
    /// Defines a method which may be invoked by PageContextActionFilter allowing controllers
    /// to modify common layout properties of the view model.
    /// </summary>
    public interface IModifyLayout
    {
        void ModifyLayout(LayoutModel layoutModel);
    }
}
