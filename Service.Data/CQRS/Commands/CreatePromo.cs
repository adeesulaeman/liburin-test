using MediatR;
using Service.Base.ViewModels.Common;
using Service.Base.ViewModels.Identity;
using Service.Data.Contracts;
using Service.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Data.CQRS.Commands
{
    public class CreatePromo : IRequest<SuccessResponseVM>
    {
        public Promo Payload { get; set; }
        public ProfileVM Actor { get; set; }
    }
    public class CreatePromoHandler : IRequestHandler<CreatePromo, SuccessResponseVM>
    {
        private readonly IPromoRepository _promoRepository;
        public CreatePromoHandler(IPromoRepository promoRepository)
        {
            _promoRepository = promoRepository;
        }

        public async Task<SuccessResponseVM> Handle(CreatePromo command, CancellationToken cancellationToken)
        {
            var result = new SuccessResponseVM();

            var request = command.Payload;
            var actor = command.Actor.Name;

            using (var role = _promoRepository.CreateTransaction((int)IsolationLevel.Serializable))
            {
                try
                {
                    Promo data = request;
                    _promoRepository.SetActor(actor);

                    var roleCreated = await _promoRepository.CreateAsync(data);

                    result.IsSuccess = true;

                    await _promoRepository.CommitTransaction(role);
                }
                catch (Exception ex)
                {
                    await _promoRepository.RollbackTransaction(role);
                    throw ex;
                }
            }

            return result;
        }
    }
}
