using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.DeliveryApi;

namespace Our.Umbraco.Meganav.Models
{
	internal class MeganavApiItem
	{

		public static MeganavApiItem Content(string title, string? queryString, string? target, Guid destinationId, string destinationType, IApiContentRoute route, int level, IEnumerable<MeganavApiItem>? children)
	       => new(LinkType.Content, url: null, queryString, title, target, destinationId, destinationType, route, level, children);

		public static MeganavApiItem External(string? title, string url, string? queryString, string? target, int level, IEnumerable<MeganavApiItem>? children)
			=> new(LinkType.External, url, queryString, title, target, null, null, null, level, children);

		private MeganavApiItem(LinkType linkType, string? url, string? queryString, string? title, string? target, Guid? destinationId, string? destinationType, IApiContentRoute? route, int level, IEnumerable<MeganavApiItem>? children)
		{
			LinkType = linkType;
			Url = url;
			QueryString = queryString;
			Title = title;
			Target = target;
			DestinationId = destinationId;
			DestinationType = destinationType;
			Route = route;
			Level = level;
			Children = children;
		}

		public string? Url { get;  }

		public string? QueryString { get; }

		public string? Title { get;  }

		public string? Target { get; }

		public Guid? DestinationId { get; }

		public string? DestinationType { get; }

		public IApiContentRoute? Route { get; }

		public LinkType LinkType { get; }

        public int Level { get; }

        public IEnumerable<MeganavApiItem> Children { get; set; } 
	}
}
