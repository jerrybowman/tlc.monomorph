tlc.monomorph
=============

A simple .net dependency framework based on attributes.

Example Usage:

1. Create an application context or obtain an existing one. This is the container holding the Beans.
A context can be named, allowing for multiple contexts to exist. You can also omit the name and
obtain (or create) an unnamed context.

        var ctx = ApplicationContext.NewContext("appcontext");

        var ctx = ApplicationContext.GetContext("appcontext");

2. Add one or more "configs" to the container. Each config is a simple POCO containing bean 
definitions that are now available from the container.

        ctx.AddConfig(new AppConfig());

3. Retrieve beans from the container.

        public MyAppClass {
          AppBusinessClass appBusiness;

          public MyAppClass(ApplicationContext ctx) {
             appBusiness = ctx.GetBean<AppBusinessClass>();
          }
          .
          .
          (more class stuff)
        }


4. Define the config class(es).

        public class AppConfig : ApplicationConfig
        {
            [Bean]
            public AppBusinessClass appBusinessClass()
            {
                return new AppBusinessClass(Context.GetBean<MoreApplicationLogic>());
            }

            [Bean]
            public MoreApplicationLogic()
            {
                return new MoreApplicationLogic();
            }
        }
