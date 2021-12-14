using Quark.Core.Configurations;
using Quark.Core.Interfaces;
using Quark.Shared.Wrapper;

namespace Quark.Server.Controllers.Utility;

[ApiController]
[Route("api/settings")]
public class SettingsController : ControllerBase
{
    private readonly IWritableOptions<LibrarySettings> _librarySettings;

    public SettingsController(IWritableOptions<LibrarySettings> librarySettings) => _librarySettings = librarySettings;

    [HttpGet("get")]
    public async Task<IActionResult> GetSettings()
    {
        return Ok(await Result<LibrarySettings>.SuccessAsync(_librarySettings.Value));
    }

    [Authorize(Policy = Permissions.Settings.Update)]
    [HttpPost("update")]
    public async Task<IActionResult> UpdateSettings(LibrarySettings settings)
    {
        _librarySettings.Update(_settings =>
            {
                _settings.CheckInDelayFinePerDay = settings.CheckInDelayFinePerDay;
                _settings.DefaultMultipleCheckoutLimit = settings.DefaultMultipleCheckoutLimit;
                _settings.DefaultExpectedCheckInInterval = settings.DefaultExpectedCheckInInterval;
            });
        return Ok(await Result.SuccessAsync("Updated settings"));
    }
}
