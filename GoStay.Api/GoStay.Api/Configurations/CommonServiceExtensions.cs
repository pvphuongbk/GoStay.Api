namespace GoStay.Api.Configurations
{
	public static class CommonServiceExtensions
	{
		public static void AddCommonServices(this IServiceCollection services)
		{
			services.AddAutoMapper(typeof(CommonServiceExtensions).Assembly);
		}
	}
}