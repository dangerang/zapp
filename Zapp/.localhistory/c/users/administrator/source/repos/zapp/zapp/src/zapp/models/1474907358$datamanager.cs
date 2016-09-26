using Coddit.Models.Entities;
using Coddit.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coddit.Models
{
    public class DataManager
    {
        CodditContext _context;

        public DataManager(CodditContext context)
        {
            _context = context;
        }

        private IdeaListVM[] GetIdeaList(string username, bool orderByRating = false)
        {
            var tmpAccount = _context.Account.SingleOrDefault(a => a.Username.CompareTo(username) == 0);

            var idealist = _context.Idea
                .Select(idea => new IdeaListVM
                {
                    Id = idea.IdeaId,
                    Description = idea.Description,
                    Subject = idea.Subject,
                    Rating = idea.Rating.Count,
                    Comment = idea.Comment.Count,
                    CreatedDate = idea.CreatedDate,
                    Members = idea.AccountIdea.Select(m => new IdeaMemberVM
                    {
                        IsCreator = idea.CreatorId == m.AccountId,
                        UserName = m.Account.Username,
                        IsActive = m.Account.IsActive
                    }).ToArray(),
                    Tags = idea.TagIdea.Where(i => i.IdeaId == idea.IdeaId).Select(t => t.Tag.Name).ToArray(),
                    HasVoted = idea.Rating.SingleOrDefault(r => r.AccountId == tmpAccount.AccountId) != null,
                    IsEnabled = true
                })
                .ToArray();

            if (orderByRating)
                idealist = idealist.OrderByDescending(i => i.Rating).ToArray();
            else
                idealist = idealist.OrderByDescending(i => i.CreatedDate).ToArray();

            return idealist;
        }

        public IdeasIndexVM GetIndexVM(string username, bool orderByRating = false)
        {
            var idealist = GetIdeaList(username, orderByRating);
            //var tmpPopular = idealist.OrderByDescending(i => i.Rating).ToList();

            //List<IdeaListVM> popularList = new List<IdeaListVM>();

            //for (int i = 0; i < 3; i++)
            //    popularList.Add(tmpPopular.ElementAt(i));

            var viewModel = new IdeasIndexVM { IdeaListModel = idealist/*, PopularIdeaListModel = popularList.ToArray()*/ };
            return viewModel;
        }

        internal void LeaveIdea(string username, int ideaId)
        {
            var tmpAccount = _context.Account.SingleOrDefault(a => a.Username.CompareTo(username) == 0);
            var tmpIdea = _context.Idea.SingleOrDefault(i => i.IdeaId == ideaId);

            var relation = _context.AccountIdea
               .SingleOrDefault(r =>
           r.IdeaId == tmpIdea.IdeaId &&
           r.AccountId == tmpAccount.AccountId);

            _context.AccountIdea.Remove(relation);
            _context.SaveChanges();
        }

        private bool CheckCreator(Idea idea, Account tmpAccount)
        {
            if (tmpAccount == null)
                return false;

            return idea.CreatorId == tmpAccount.AccountId;
        }

        internal IdeaEditVM GetEditIdea(int ideaId)
        {
            var tmpIdea = _context.Idea.SingleOrDefault(i => i.IdeaId == ideaId);

            return new IdeaEditVM
            {
                Subject = tmpIdea.Subject,
                Description = tmpIdea.Description
            };
        }

        internal void EditIdea(int ideaId, IdeaEditVM viewModel)
        {
            var tmpIdea = _context.Idea.SingleOrDefault(n => n.IdeaId == ideaId);

            tmpIdea.Subject = viewModel.Subject;
            tmpIdea.Description = viewModel.Description;

            _context.SaveChanges();
        }

        internal void AddComment(int ideaId, string username, string text)
        {
            int tmpAccountId = _context.Account.SingleOrDefault(a => a.Username == username).AccountId;

            _context.Comment.Add(new Comment
            {
                AccountId = tmpAccountId,
                IdeaId = ideaId,
                Text = text,
                CreatedDate = DateTime.Now
            });

            _context.SaveChanges();
        }

        internal bool RemoveComment(int commentId)
        {
            var tmpComment = _context.Comment.SingleOrDefault(c => c.CommentId == commentId);

            if (tmpComment == null)
                return false;

            _context.Comment.Remove(tmpComment);

            _context.SaveChanges();

            return true;
        }

        internal CommentListVM[] GetCommentList(int ideaId, string username)
        {
            var tmpList = _context.Comment
                .Where(c => c.IdeaId == ideaId)
                .Select(c => new
                {
                    CommentId = c.CommentId,
                    UserId = c.AccountId,
                    CommentText = c.Text,
                    CreatedDate = c.CreatedDate
                })
                .ToArray();

            var commentList = tmpList.Select(c => new CommentListVM
            {
                CommentId = c.CommentId,
                Text = c.CommentText,
                Username = _context.Account.SingleOrDefault(a => a.AccountId == c.UserId).Username,
                CreatedDate = c.CreatedDate
            })
            .OrderByDescending(c => c.CreatedDate)
            .ToArray();

            foreach (var comment in commentList)
                comment.IsCreator = comment.Username.CompareTo(username) == 0;

            return commentList;
        }

        public void AddAccount(LandingRegisterVM viewModel)
        {
            if (_context.Account.SingleOrDefault(n => n.Username.CompareTo(viewModel.NewUsername) == 0) != null)
                return;

            _context.Account.Add(new Account
            {
                Username = viewModel.NewUsername,
                Email = viewModel.Email,
                IsProgrammer = viewModel.IsProgrammer,
                IsActive = true
            });

            _context.SaveChanges();
        }

        public bool JoinIdea(string username, int ideaId)
        {
            bool success = false;

            var tmpAccount = _context.Account.SingleOrDefault(a => a.Username.CompareTo(username) == 0);
            var tmpIdea = _context.Idea.SingleOrDefault(i => i.IdeaId == ideaId);

            var relation = _context.AccountIdea
                .SingleOrDefault(r =>
            r.IdeaId == tmpIdea.IdeaId &&
            r.AccountId == tmpAccount.AccountId);

            if (relation == null)
            {
                tmpIdea
                    .AccountIdea
                    .Add(new AccountIdea
                    {
                        AccountId = tmpAccount.AccountId,
                        IdeaId = tmpIdea.IdeaId
                    });

                _context.SaveChanges();

                success = true;
            }

            return success;
        }

        internal void RemoveUpvote(int ideaId, string username)
        {
            var idea = _context.Idea.SingleOrDefault(i => i.IdeaId == ideaId);
            var tmpAccount = _context.Account.SingleOrDefault(a => a.Username.CompareTo(username) == 0);

            var rating = _context.Rating.SingleOrDefault(r => r.IdeaId == idea.IdeaId && r.AccountId == tmpAccount.AccountId);

            if (rating != null)
            {
                _context.Rating.Remove(rating);
                _context.SaveChanges();
            }
        }

        internal bool Upvote(int ideaId, string username)
        {
            bool success = false;

            var idea = _context.Idea.SingleOrDefault(i => i.IdeaId == ideaId);
            var tmpAccount = _context.Account.SingleOrDefault(a => a.Username.CompareTo(username) == 0);

            if (_context.Rating.SingleOrDefault(r => r.IdeaId == idea.IdeaId && r.AccountId == tmpAccount.AccountId) == null)
            {
                _context.Rating.Add(new Rating
                {
                    IdeaId = idea.IdeaId,
                    AccountId = tmpAccount.AccountId
                });

                _context.SaveChanges();

                success = true;
            }

            return success;
        }

        public void RemoveIdea(int ideaId)
        {
            var tmpIdea = _context.Idea.SingleOrDefault(i => i.IdeaId == ideaId);
            var relation = _context.AccountIdea.Where(r => r.IdeaId == ideaId).ToArray();

            _context.AccountIdea.RemoveRange(relation);
            _context.Idea.Remove(tmpIdea);

            _context.SaveChanges();
        }

        public AccountProfileVM GetProfile(string username)
        {
            var tmpAccount = _context.Account
                .Include(o => o.Idea)
                .Include(o => o.Comment)
                .Include(o => o.Rating)
                .SingleOrDefault(i => i.Username.CompareTo(username) == 0);

            var profile = new AccountProfileVM
            {
                Id = tmpAccount.AccountId,
                Username = tmpAccount.Username,
                Email = tmpAccount.Email,
                Description = GetDescription(tmpAccount.Description),
                ProfilePic = GetProfilePic(tmpAccount.ImageSrc),
                IsProgrammer = (bool)tmpAccount.IsProgrammer,
                MyIdeas = tmpAccount.Idea.Select(idea => new AccountProfileIdeaVM
                {
                    Subject = idea.Subject,
                    IsCreator = CheckCreator(idea, tmpAccount),
                    CreatedDate = idea.CreatedDate,
                    Description = idea.Description,
                    Rating = idea.Rating.Count,
                    Comment = idea.Comment.Count
                })
                .OrderByDescending(o => o.CreatedDate)
                .ToArray()
            };

            return profile;
        }

        private string GetDescription(string description)
        {
            string result = "";

            if (description != null)
                result = description;

            return result;
        }

        private string GetProfilePic(string imageSrc)
        {
            string result = @"http://apisrex.net/wp-content/uploads/2016/06/avatar.png";

            if (imageSrc != null)
                result = $"@{imageSrc}";

            return result;
        }

        internal void RemoveAccount(string username)
        {
            var tmpAccount = _context.Account.SingleOrDefault(n => n.Username.CompareTo(username) == 0);
            var createdIdeas = _context.Idea.Where(i => i.CreatorId == tmpAccount.AccountId).ToArray();

            tmpAccount.IsActive = false;

            foreach (var idea in createdIdeas)
                idea.IsEnabled = false;

            _context.SaveChanges();
        }

        internal AccountEditVM GetEditProfile(string username)
        {
            var profile = GetProfile(username);

            return new AccountEditVM
            {
                Description = profile.Description,
                Email = profile.Email,
                IsProgrammer = profile.IsProgrammer,
                Tags = profile.Tags
            };
        }

        internal void EditAccount(AccountEditVM viewModel, string username)
        {
            var tmpAccount = _context.Account.SingleOrDefault(n => n.Username.CompareTo(username) == 0);

            tmpAccount.Email = viewModel.Email;
            tmpAccount.IsProgrammer = viewModel.IsProgrammer;
            tmpAccount.Description = viewModel.Description;

            _context.SaveChanges();
        }

        public void AddIdea(IdeaCreateVM viewModel, string[] newTags, string username)
        {
            Account creator = _context.Account.SingleOrDefault(n => n.Username.CompareTo(username) == 0);

            var idea = new Idea
            {
                Description = viewModel.Description,
                Subject = viewModel.Subject,
                CreatorId = creator.AccountId,
                CreatedDate = DateTime.Now,
                IsEnabled = true
            };

            var member = new AccountIdea
            {
                AccountId = creator.AccountId,
            };

            idea.AccountIdea.Add(member);
            _context.Idea.Add(idea);

            AddTags(newTags, idea);

            _context.SaveChanges();
        }

        //public void AddIdea(LandingProjectVM viewModel, string[] newTags, string username)
        //{
        //    Account creator = _context.Account.SingleOrDefault(n => n.Username.CompareTo(username) == 0);

        //    var idea = new Idea
        //    {
        //        Description = viewModel.Description,
        //        Subject = viewModel.Subject,
        //        CreatorId = creator.AccountId,
        //        CreatedDate = DateTime.Now,
        //        IsEnabled = true
        //    };

        //    var member = new AccountIdea
        //    {
        //        AccountId = creator.AccountId,
        //    };

        //    idea.AccountIdea.Add(member);
        //    _context.Idea.Add(idea);

        //    AddTags(newTags, idea);

        //    _context.SaveChanges();
        //}

       
        private void AddTags(string[] newTags, Idea idea)
        {
            foreach (var tag in newTags)
            {
                var tmpTag = _context.Tag.SingleOrDefault(t => t.Name.Equals(tag, StringComparison.OrdinalIgnoreCase));

                if (tmpTag == null)
                {
                    tmpTag = new Tag
                    {
                        Name = tag
                    };

                    _context.Tag.Add(tmpTag);
                }

                var tagsIdea = new TagIdea
                {
                    IdeaId = idea.IdeaId,
                    TagId = tmpTag.TagId
                };

                _context.TagIdea.Add(tagsIdea);
            }
        }
    }
}