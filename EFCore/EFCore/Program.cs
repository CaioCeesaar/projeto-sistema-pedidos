using System.Formats.Tar;
using Microsoft.EntityFrameworkCore;

using var db = new ApplicationContext();
var existe = db.Database.GetPendingMigrations().Any();
if (existe) 
{
    // Aqupodemos fazer um tratamento de erro, caso exista alguma migração pendente
}
