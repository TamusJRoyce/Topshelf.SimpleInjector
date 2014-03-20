Topshelf.SimpleInjector
=======================

Put your apps on the Topshelf, with the power of SimpleInjector! Topshelf.SimpleInjector provides extensions to construct your TopShelf service class from the SimpleInjector IoC container.

Install
=======================
Install

Example Usage
=======================
```csharp
    class Program
    {
        private static readonly Container _container = new Container();
        
        static void Main(string[] args)
        {
            //Register services
            _container.Register<ISampleDependency, SampleDependency>();
            _container.Register<ISampleDependency2, SampleDependency2>();
            //This does not need to be explicitly registered
            _container.Register<SampleService>();

            //Check container for errors
            _container.Verify();

            HostFactory.Run(config =>
            {
                // Pass it to Topshelf
                config.UseSimpleInjector(_container);

                config.Service<SampleService>(s =>
                {
                    // Let Topshelf use it
                    s.ConstructUsingSimpleInjector();
                    s.WhenStarted((service, control) => service.Start());
                    s.WhenStopped((service, control) => service.Stop());
                });
            });
        }

        public class SampleService
        {
            private readonly ISampleDependency _sample;
            private readonly ISampleDependency2 _sample2;

            public SampleService(ISampleDependency sample, ISampleDependency2 sample2)
            {
                _sample = sample;
                _sample2 = sample2;
            }

            public bool Start()
            {
                Console.WriteLine("Sample Service Started.");
                Console.WriteLine("Sample Dependency: {0}", _sample);
                Console.WriteLine("Sample Dependency2: {0}", _sample2);
                return _sample != null && _sample2 != null;
            }

            public bool Stop()
            {
                return _sample != null && _sample2 != null;
            }
        }

        public interface ISampleDependency
        {
        }

        public class SampleDependency : ISampleDependency
        {
        }

        public interface ISampleDependency2
        {
        }

        public class SampleDependency2 : ISampleDependency2
        {
        }
    }
```

References
=======================
- [Topshelf](http://topshelf-project.com)
- [SimpleInjector](https://simpleinjector.codeplex.com)
- [Topshelf.Autofac](https://github.com/alexandrnikitin/Topshelf.Autofac)

Copyright & License
=======================
Copyright 2014 tynor88

Licensed under the [MIT License](https://github.com/tynor88/Topshelf.SimpleInjector/blob/master/LICENSE)
