1. в студии открываем консоль: Tools -> NuGet Package Manager -> Package Manager Console
2. ¬водим команду: nuget spec SampleNugetPackage
3. созданный файл SampleNugetPackage.nuspec, копируем в папку проекта SampleNugetPackage.
   старый файл оставл€ем дл€ нагл€дности.
4. в созданный файл SampleNugetPackage.nuspec, вносим изменени€.
5. ¬водим комманду: nuget pack .\SampleNugetPackage\SampleNugetPackage.csproj -Properties Configuration=Release -Build