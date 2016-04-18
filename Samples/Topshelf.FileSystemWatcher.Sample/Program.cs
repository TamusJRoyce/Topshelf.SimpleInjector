using System;
using System.IO;

namespace Topshelf.FileSystemWatcher.Sample
{
    internal class Program
    {
        private static readonly string _testDir = Directory.GetCurrentDirectory() + @"\test\";

        private static void Main(string[] args)
        {
            HostFactory.Run(config =>
            {
                config.UseTestHost();

                config.Service<Program>(s =>
                {
                    s.ConstructUsing(() => new Program());
                    s.WhenStarted((service, host) =>
                    {
                        if (!Directory.Exists(_testDir))
                            Directory.CreateDirectory(_testDir);
                        using (FileStream fs = File.Create(_testDir + "testfile.ext"))
                        {
                        }
                        return true;
                    });
                    s.WhenStopped((service, host) =>
                    {
                        return true;
                    });
                    s.WhenFileSystemCreated(configurator =>
                        configurator.AddDirectory(dir =>
                        {
                            dir.Path = _testDir;
                            dir.CreateDir = true;
                            dir.NotifyFilters = NotifyFilters.FileName;
                        }), FileSystemCreated);

                    s.WhenFileSystemRenamed(configurator =>
                        configurator.AddDirectory(dir =>
                        {
                            dir.Path = _testDir;
                            dir.CreateDir = true;
                        }), FileSystemRenamed);
                });
            });
        }

        private static void FileSystemCreated(TopshelfFileSystemEventArgs topshelfFileSystemEventArgs)
        {
            Console.WriteLine("New file created! ChangeType = {0} FullPath = {1} Name = {2} FileSystemEventType {3}", topshelfFileSystemEventArgs.ChangeType, topshelfFileSystemEventArgs.FullPath, topshelfFileSystemEventArgs.Name, topshelfFileSystemEventArgs.FileSystemEventType);
        }

	    private static void FileSystemRenamed(RenamedEventArgs renamedEventArgs)
	    {
                Console.WriteLine("File renamed! ChangeType = {0} FullPath = {1} Name = {2} OldName = {3}", renamedEventArgs.ChangeType, renamedEventArgs.FullPath, renamedEventArgs.Name, renamedEventArgs.OldName);
	    }
    }
}