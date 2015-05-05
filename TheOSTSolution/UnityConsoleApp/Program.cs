sing System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace UnityConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //创建一个代表 IoC 容器的 UnityContainer 对象。
            IUnityContainer container = new UnityContainer();
            //根据配置文件对其进行初始化。
            UnityConfigurationSection configuration = 
                System.Configuration.ConfigurationManager.GetSection(UnityConfigurationSection.SectionName) as UnityConfigurationSection;
            configuration.Configure(container, "defaultContainer");
	    //
            A a = container.Resolve<IA>() as A;
            if (null != a)
            {
                Console.WriteLine("a.B is null? -{0}", a.B == null ? "yes" : "no");
                Console.WriteLine("a.C is null? -{0}", a.C == null ? "yes" : "no");
                Console.WriteLine("a.D is null? -{0}", a.D == null ? "yes" : "no");
            }

            Console.ReadKey();
        }
    }
    public interface IA { }
    public interface IB { }
    public interface IC { }
    public interface ID { }

    public class A : IA
    {
        public IB B { get; set; }

        //属性注入
        [Dependency]
        public IC C { get; set; }
        public ID D { get; set; }

        public A(IB b)
        {
            //构造函数注入
            this.B = b;
        }

        [InjectionMethod]
        public void Initialize(ID d)
        {
            // 方法注入
            this.D = d;
        }
    }
    public class B : IB { }
    public class C : IC { }
    public class D : ID { }
}
