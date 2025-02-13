using System;
using uni.learn.BussinesLogic.Data;
using uni.learn.core.Entities;
using uni.learn.core.Entity;

namespace uni.learn.BussinesLogic.Context.Data;

public class MainDbContextData
{
    public static async Task seedDataAsync(MainDbContext context)
    {
        try{
           if(!context.Tema.Any())
        {
            var temas = new List<Temas>
            {
                new Temas
                {
                    Nombre = "Programación",
                },
                new Temas
                {
                    Nombre = "Matemáticas",
                },
                new Temas
                {
                    Nombre = "Ciencias",
                },
                new Temas
                {
                    Nombre = "Idiomas",
                },
                new Temas
                {
                    Nombre = "Diseño",
                }
            };

            await context.Tema.AddRangeAsync(temas);
            await context.SaveChangesAsync();
        }
            
        }catch(Exception e){
            System.Console.WriteLine(e);
        }

    }

}
