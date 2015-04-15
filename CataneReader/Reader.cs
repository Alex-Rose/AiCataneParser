using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Windows.Threading;
using System.Windows;

namespace CataneReader
{
    class Reader
    {
        protected FileSystemWatcher _watcher;
        protected string _filePath;
        protected string _fileName;
        protected List<string> _maps;

        public event EventHandler<List<string>> Parsed;

        public Reader(string path)
        {
            _filePath = path;
            var directory = Path.GetDirectoryName(path);

            _fileName = Path.GetFileName(path);

            _watcher = new FileSystemWatcher(directory);

            _watcher.Changed += _watcher_Changed;
            _watcher.Created += _watcher_Created;

            _watcher.EnableRaisingEvents = true;
        }

        void _watcher_Created(object sender, FileSystemEventArgs e)
        {
            
        }

        void _watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.Name == _fileName)
            {
                Console.WriteLine("File changed");

                Thread.Sleep(1000);
                Parse();

                Parsed.Invoke(this, _maps);
            }
        }

        public void Parse()
        {
            try
            {
                var lines = File.ReadAllLines(_filePath);

                var maps = new List<string>();

                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i] == "�tat apr�s le placement initial:" || lines[i] == "�tat � la fin du tour")
                    {
                        string map = "";
                        i++;
                        while (i < lines.Length && !lines[i].StartsWith("Joueur"))
                        {
                            map += lines[i] + "\r\n";
                            i++;
                        }

                        maps.Add(map);
                    }
                }

                _maps = maps;
            }
            catch (Exception)
            {
                Thread.Sleep(1000);
                Parse();
            }
        }
    }
}
