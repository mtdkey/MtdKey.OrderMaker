using MtdKey.EmailBuilder.EmailBlocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace MtdKey.EmailBuilder.Tests
{
    public class EmailDesignerTests(ITestOutputHelper testOutputHelper, ServicesHelper fixture)
        : TestBed<ServicesHelper>(testOutputHelper, fixture)
    {
        [Fact]
        public async Task Greate_Layout_And_Heading()
        {
            var templateStorage = _fixture.GetService<ITemplateStorage>(_testOutputHelper)!;
            var emailDesigner = new EmailDesigner();
            var layout = new LayoutBlock(templateStorage);
            await layout.CreateAsync();           

            string title = "Title";
            var heading = new HeadingBlock(templateStorage);
            await heading.CreateAsync(title);

            var container = new ContainerBlock(templateStorage);
            await container.CreateAsync([heading]);

            emailDesigner.AddLayout(layout)
                .AddBlock(container);

            var template = emailDesigner.Build();
            Assert.Contains("<html>", template);
            Assert.Contains(title,template);
        }


        [Fact]
        public async Task Greate_Columns_Button_And_Image()
        {
            var templateStorage = _fixture.GetService<ITemplateStorage>(_testOutputHelper)!;
            var emailDesigner = new EmailDesigner();

            var button = new ButtonBlock(templateStorage);
            string caption = "Start";
            string buttomeUrl = "https://localhost";
            await button.CreateAsync(caption, buttomeUrl);

            var image = new ImageBlock(templateStorage);
            string imageUrl = "imageUrl";
            await image.CreateAsync(imageUrl);

            var columns = new ColumnsBlock(templateStorage);
            await columns.CreateAsync(button, image);
          
            var template = emailDesigner.AddBlock(columns).Build();
            Assert.Contains(imageUrl, template);
            Assert.Contains(caption, template);
        }

        [Fact]
        public async Task Create_TextBlok_With_Divider()
        {
            var templateStorage = _fixture.GetService<ITemplateStorage>(_testOutputHelper)!;
            var emailDesigner = new EmailDesigner();

            var textLine1 = new TextBlock(templateStorage);
            await textLine1.CreateAsync(nameof(textLine1));
            var textLine2 = new TextBlock(templateStorage);
            await textLine2.CreateAsync(nameof(textLine2));

            var divider = new DividerBlock(templateStorage);
            await divider.CreateAsync();

            var container = new ContainerBlock(templateStorage);
            await container.CreateAsync([textLine1, divider, textLine2]);

            var template = emailDesigner.AddBlock(container).Build();

            Assert.Contains("<hr", template);
            Assert.Contains(nameof(textLine1), template);
            Assert.Contains(nameof(textLine2), template);
        }

        [Fact]
        public async Task Create_LinkBlock_With_Spacer()
        {
            var templateStorage = _fixture.GetService<ITemplateStorage>(_testOutputHelper)!;
            var emailDesigner = new EmailDesigner();

            var linkk1 = new LinkBlock(templateStorage);
            await linkk1.CreateAsync(nameof(linkk1),$"{nameof(linkk1)}-url");
            var linkk2 = new LinkBlock(templateStorage);
            await linkk2.CreateAsync(nameof(linkk2), $"{nameof(linkk2)}-url");

            var spacer = new SpacerBlock(templateStorage);
            await spacer.CreateAsync();

            var container = new ContainerBlock(templateStorage);
            await container.CreateAsync([linkk1, spacer, linkk2]);

            var template = emailDesigner
                .AddBlock(container)
                .Build();

            Assert.Contains("&nbsp", template);
            Assert.Contains(nameof(linkk1), template);
            Assert.Contains($"{nameof(linkk1)}-url", template);
            Assert.Contains(nameof(linkk2), template);
            Assert.Contains($"{nameof(linkk2)}-url", template);
        }


        [Fact]
        public async Task Create_InfoLine()
        {
            var templateStorage = _fixture.GetService<ITemplateStorage>(_testOutputHelper)!;
            var emailDesigner = new EmailDesigner();

            var infoLine = new InfoLineBlock(templateStorage);
            await infoLine.CreateAsync(nameof(infoLine));

            var container = new ContainerBlock(templateStorage);
            await container.CreateAsync([infoLine]);

            var template = emailDesigner
                .AddBlock(container)
                .Build();

            Assert.Contains("&#8505;", template);
        }
    }
}
