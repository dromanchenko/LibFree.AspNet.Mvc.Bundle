﻿using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBundler.Core.Abstractions;

namespace XBundler.Core.TagHelpers
{
	public abstract class BundleTagHelper : TagHelper
	{
		private ILoggerFactory _loggerFactory;
		private IBundleRuntime _bundleRuntime;

		[HtmlAttributeName("virtualPath")]
		public string VirtualPath { get; set; }

		[HtmlAttributeName("environments")]
		public string TargetEnvironments { get; set; }

		public BundleTagHelper(ILoggerFactory loggerFactory, IBundleRuntime bundleRuntime)
		{
			_loggerFactory = loggerFactory;
			_bundleRuntime = bundleRuntime;
        }

		protected abstract ILogger GetLogger(ILoggerFactory loggerFactory);
		protected abstract string GetLoggerMessagesPrefix();
		protected abstract IEnumerable<string> ParseHtml(string content);
		protected abstract BundleType GetBundleType();

		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			var logger = GetLogger(_loggerFactory);
			var loggerMessagesPrefix = GetLoggerMessagesPrefix();
			logger.LogDebug("{0}: running", loggerMessagesPrefix);

			var bundle = _bundleRuntime.GetBundle(VirtualPath);
			if (bundle == null)
			{
				var bundleType = GetBundleType();
				var childContent = await output.GetChildContentAsync();
				bundle = _bundleRuntime.CreateBundle(bundleType, VirtualPath, TargetEnvironments, ParseHtml(childContent.GetContent()), loggerMessagesPrefix);
			}

			output.SuppressOutput();

			if (bundle.ShouldBundle)
			{
				output.Content.SetHtmlContent(bundle.GetLHtmlTags());
			}
			else
			{
				var childContent = (await output.GetChildContentAsync()).GetContent();
				output.Content.SetHtmlContent(childContent);
			}
		}
	}
}