namespace Simplify.Web.Swagger
{
	/// <summary>
	/// provides controller action names
	/// </summary>
	public class ControllerActionNames
	{
		/// <summary>
		/// Initializes ControllerActionNames
		/// </summary>
		/// <param name="name">Controller full name</param>
		/// <param name="groupName">Controller group name</param>
		/// <param name="summary">Controller summary</param>
		public ControllerActionNames(string name, string groupName, string? summary = null)
		{
			Name = name;
			GroupName = groupName;
			Summary = summary;
		}

		/// <summary>
		/// Controller full name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Controller group name
		/// </summary>
		public string GroupName { get; set; }

		/// <summary>
		/// Controller summary
		/// </summary>
		public string? Summary { get; set; }
	}
}