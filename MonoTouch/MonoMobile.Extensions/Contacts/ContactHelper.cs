using System;
using System.Linq;
using MonoTouch.AddressBook;

namespace Xamarin.Contacts
{
	internal static class ContactHelper
	{
		internal static Contact GetContact (ABPerson person)
		{
			Contact contact = new Contact (person.Id.ToString())
			{
				DisplayName = person.ToString(),
				Prefix = person.Prefix,
				FirstName = person.FirstName,
				MiddleName = person.MiddleName,
				LastName = person.LastName,
				Suffix = person.Suffix,
				Nickname = person.Nickname
			};
			
			contact.Notes = (person.Note != null) ? new [] { person.Note } : new string[0];

			contact.Emails = person.GetEmails().Select (e => new Email
			{
				Address = e.Value,
				Type = EmailType.Other,
				Label = ABAddressBook.LocalizedLabel (ABLabel.Other)
			}).ToArray();
			
			contact.Phones = person.GetPhones().Select (p => new Phone
			{
				Number = p.Value,
				Type = GetPhoneType (p),
				Label = ABAddressBook.LocalizedLabel (p.Label)
			}).ToArray();
			
			return contact;
		}
		
		internal static PhoneType GetPhoneType (ABMultiValueEntry<string> phone)
		{
			if (phone.Label == ABLabel.Home)
				return PhoneType.Home;
			if (phone.Label == ABLabel.Work)
				return PhoneType.Work;
			if (phone.Label == ABPersonPhoneLabel.Mobile || phone.Label == ABPersonPhoneLabel.iPhone)
				return PhoneType.Mobile;
			if (phone.Label == ABPersonPhoneLabel.Pager)
				return PhoneType.Pager;
			if (phone.Label == ABPersonPhoneLabel.HomeFax)
				return PhoneType.HomeFax;
			if (phone.Label == ABPersonPhoneLabel.WorkFax)
				return PhoneType.WorkFax;
			
			return PhoneType.Other;
		}
	}
}

