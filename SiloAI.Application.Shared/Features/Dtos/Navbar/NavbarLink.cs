namespace SiloAI.Application.Shared.Features;

public class NavbarAllTitle
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string IconName { get; set; }
    public List<NavbarCategory> Children { get; set; } = new();
}

public class NavbarLink
{
    public NavbarLink()
    {

    }
    public NavbarLink(string url, string title)
    {
        Url = url;
        Title = title;
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
}

public class NavbarCategory
{
    public NavbarCategory()
    {

    }

    public NavbarCategory(string title, List<NavbarLink> children)
    {
        Title = title;

        Children = children;
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public List<NavbarLink> Children { get; set; } = new();
}
