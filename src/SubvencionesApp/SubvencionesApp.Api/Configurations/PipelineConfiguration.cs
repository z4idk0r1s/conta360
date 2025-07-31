namespace SubvencionesApp.Api.Configurations
{
    public static class PipelineConfiguration
    {
        public static void ConfigurePipeline(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Subvenciones API V1");
                    c.RoutePrefix = string.Empty;
                });
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseResponseCompression();
            app.UseHttpsRedirection();
            app.UseCors();
            
            app.UseRouting();
            app.UseAuthorization();
            
            app.MapControllers();
            app.MapHealthChecks("/health");

            app.MapGet("/info", () => new
            {
                Application = "SubvencionesApp",
                Version = "1.0.0",
                Environment = app.Environment.EnvironmentName,
                Timestamp = DateTime.UtcNow
            });
        }
    }
}