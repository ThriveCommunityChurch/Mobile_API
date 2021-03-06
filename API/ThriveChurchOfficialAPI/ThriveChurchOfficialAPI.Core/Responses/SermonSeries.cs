﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ThriveChurchOfficialAPI.Core
{
    /// <summary>
    /// C'tor
    /// </summary>
    public class SermonSeries
    {
        public SermonSeries()
        {
            StartDate = null;
            EndDate = null;
            Messages = null;
            Name = null;
            Year = null;
            Slug = null;
            Thumbnail = null;
            ArtUrl = null;
            LastUpdated = null;
        }

        /// <summary>
        /// ObjectId notation from Mongo
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// The name of the sermon series
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "")]
        public string Name { get; set; }

        /// <summary>
        /// This is a string notation for the year that the series is taking place
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// The starting date of the sermon series - we will ignore the time
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// The ending date of the sermon series - we will ignore the time
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// This is a reference to the url link on the website (so these need to stay unique)
        /// for example -> domain.org/{insert-slug-here}
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// The direct URL to the thumbnail for this sermon series
        /// </summary>
        public string Thumbnail { get; set; }

        /// <summary>
        /// The direct URL to the full res art for this sermon series
        /// </summary>
        public string ArtUrl { get; set; }

        /// <summary>
        /// Used as a timestamp to indicate the last time that this object was updated
        /// </summary>
        public DateTime? LastUpdated { get; set; }

        /// <summary>
        /// A collection of Messages spoken / given by someone within this sermon series
        /// </summary>
        public IEnumerable<SermonMessage> Messages { get; set; }

        /// <summary>
        /// Validate the request object
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static ValidationResponse ValidateRequest(SermonSeries request)
        {
            if (request == null)
            {
                return new ValidationResponse(true, SystemMessages.EmptyRequest);
            }

            if (string.IsNullOrEmpty(request.Name))
            {
                return new ValidationResponse(true, string.Format(SystemMessages.NullProperty, "Name"));
            }
            if (string.IsNullOrEmpty(request.ArtUrl))
            {
                return new ValidationResponse(true, string.Format(SystemMessages.NullProperty, "ArtUrl"));
            }
            if (string.IsNullOrEmpty(request.Year))
            {
                return new ValidationResponse(true, string.Format(SystemMessages.NullProperty, "Year"));
            }
            if (request.StartDate == null)
            {
                return new ValidationResponse(true, string.Format(SystemMessages.NullProperty, "StartDate"));
            }
            if (string.IsNullOrEmpty(request.Thumbnail))
            {
                return new ValidationResponse(true, string.Format(SystemMessages.NullProperty, "Thumbnail"));
            }
            if (string.IsNullOrEmpty(request.Slug))
            {
                return new ValidationResponse(true, string.Format(SystemMessages.NullProperty, "Slug"));
            }

            // there's no guarantee that a requested value here will keep the same value
            if (request.LastUpdated != null)
            {
                request.LastUpdated = null;
            }

            // messages must at least be an object, it should not be null
            if (request.Messages == null)
            {
                request.Messages = new List<SermonMessage>();
            }

            if (request.StartDate != null && request.EndDate != null)
            {
                // make sure that the dates are chronological
                if (request.StartDate > request.EndDate)
                {
                    return new ValidationResponse(true, SystemMessages.EndDateMustBeAfterStartDate);
                }
            }

            return new ValidationResponse("Success!");
        }
    }
}