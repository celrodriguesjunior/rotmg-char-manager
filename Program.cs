using System;
using System.Windows.Forms;
using RotmgManager.Persistence;
using RotmgManager.Services;

namespace RotmgManager;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        var repository = new GameDataRepository();
        var data = repository.Load();
        var service = new CharacterService(data);

        Application.Run(new MainForm(repository, data, service));
    }
}
