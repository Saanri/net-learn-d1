1. � ������ ��������� �������: Tools -> NuGet Package Manager -> Package Manager Console
2. ������ �������: nuget spec SampleNugetPackage
3. ��������� ���� SampleNugetPackage.nuspec, �������� � ����� ������� SampleNugetPackage.
   ������ ���� ��������� ��� �����������.
4. � ��������� ���� SampleNugetPackage.nuspec, ������ ���������.
5. ������ ��������: nuget pack .\SampleNugetPackage\SampleNugetPackage.csproj -Properties Configuration=Release -Build