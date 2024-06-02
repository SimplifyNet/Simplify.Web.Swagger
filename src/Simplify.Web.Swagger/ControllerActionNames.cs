namespace Simplify.Web.Swagger
{
	/// <summary>
	/// Provides the controller action names.
	/// </summary>
	/// <remarks>
	/// Initializes ControllerActionNames.
	/// </remarks>
	/// <param name="name">Controller full name</param>
	/// <param name="groupName">Controller group name</param>
	/// <param name="summary">Controller summary</param>
	public class ControllerActionNames(string name, string groupName, string? summary = null)
	{
		/// <summary>
		/// Gets or sets the controller full name
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get; set; } = name;

		/// <summary>
		/// Gets or sets the controller group name
		/// </summary>
		/// <value>
		/// The name of the group.
		/// </value>
		public string GroupName { get; set; } = groupName;

		/// <summary>
		/// Gets or sets the controller summary
		/// </summary>
		/// <value>
		/// The summary.
		/// </value>
		public string? Summary { get; set; } = summary;
	}
}