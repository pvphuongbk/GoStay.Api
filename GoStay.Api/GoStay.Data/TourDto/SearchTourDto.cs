﻿using GoStay.Data.Enums;

namespace GoStay.Data.TourDto
{
    public class SearchTourDto
    {
        public int Id { get; set; }
        public int IdTourStyle { get; set; }
        public int IdTourTopic { get; set; }

        public string TourName { get; set; }
        public string TourStyle { get; set; }
        public string TourTopic { get; set; }
        public int Rating { get; set; }
        public int Style { get; set; }

        public int TourSize { get; set; }

        public int IdDistrictFrom { get; set; }
        public string? DistrictFrom { get; set; }
        public string? IdDistrictTos
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                    IdDistrictTo = new List<int>();
                else
                {
                    var temp= value.Split(';').ToList();
                    foreach (var item in temp)
                    {
                        IdDistrictTo.Add(int.Parse(item));
                    }

                }
            }
        }

        public List<int>? IdDistrictTo { get; set; } = new List<int>();

        public string? DistrictTos
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                    DistrictTo = new List<string>();
                else
                {
                    DistrictTo = value.Split(';').ToList();

                }
            }
        }

        public List<string>? DistrictTo { get; set; } = new List<string>();

        public DateTime StartDate { get; set; }
        public string StartTime { get; set; }

        public double Price { get; set; }
        public int Discount { get; set; }
        public double ActualPrice { get; set; }
        public int Total { get; set; }
        public int TotalPicture { get; set; }
        private string Urls
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                    Pictures = new List<string>();
                else
                {
                    Pictures = value.Split(';').ToList();

                }
            }
        }
        public List<string> Pictures { get; set; } = new List<string>();
        public string Slug { get; set; }
    }

    public class TourInCompareDto
    {
        public int Id { get; set; }
        public string TourName { get; set; }
        public int Rating { get; set; }
        public int TourSize { get; set; }
        public DateTime StartDate { get; set; }
        public double Price { get; set; }
        public int Discount { get; set; }
        public double ActualPrice { get; set; }
        public string? DistrictFrom { get; set; }
        public string? ProvinceFrom { get; set; }
        public string StartTime { get; set; }
        public string TourStyle { get; set; }
        public string TourTopic { get; set; }

        public string Titles
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                    Title = new List<string>();
                else
                {
                    Title = value.Split(';').ToList();

                }
            }
        }
        public List<string>? Title { get; set; } = new List<string>();
        public string? DistrictTos
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                    DistrictTo = new List<string>();
                else
                {
                    DistrictTo = value.Split(';').ToList();

                }
            }
        }

        public List<string>? DistrictTo { get; set; } = new List<string>();

        public string? ProvinceTos
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                    ProvinceTo = new List<string>();
                else
                {
                    ProvinceTo = value.Split(';').ToList();

                }
            }
        }

        public List<string>? ProvinceTo { get; set; } = new List<string>();

        private string Urls
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                    Pictures = new List<string>();
                else
                {
                    Pictures = value.Split(';').ToList();

                }
            }
        }

        public List<string> Pictures { get; set; } = new List<string>();
        public string Slug { get; set; }
    }
}
