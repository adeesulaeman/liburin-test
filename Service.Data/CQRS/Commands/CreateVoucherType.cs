using MediatR;
using Service.Base.ViewModels.Common;
using Service.Base.ViewModels.Identity;
using Service.Data.Contracts;
using Service.Data.Models;
using Service.Data.ViewModels.VoucherType;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Data.CQRS.Commands
{
    public class CreateVoucherType: IRequest<SuccessResponseVM>
    {
        public CreateVoucherTypeRequestVM Payload { get; set; }
        public ProfileVM Actor { get; set; }
    }

    public class CreateVoucherTypeHandler : IRequestHandler<CreateVoucherType, SuccessResponseVM>
    {
        private readonly IVoucherTypeRepository _vocTypeRepository;
        //private readonly IPermissionRepository _permissionRepository;

        public CreateVoucherTypeHandler(IVoucherTypeRepository vocTypeRepository)
        {
            _vocTypeRepository = vocTypeRepository;
        }

        public async Task<SuccessResponseVM> Handle(CreateVoucherType command, CancellationToken cancellationToken)
        {
            var result = new SuccessResponseVM();

            var request = command.Payload.VoucherType;
            var actor = command.Actor.Name;

            using (var role = _vocTypeRepository.CreateTransaction((int)IsolationLevel.Serializable))
            {
                try
                {
                    VoucherType data = new VoucherType
                    {
                        Name = request.Name
                    };
                    _vocTypeRepository.SetActor(actor);

                    var roleCreated = await _vocTypeRepository.CreateAsync(data);

                    result.IsSuccess = true;

                    await _vocTypeRepository.CommitTransaction(role);
                }
                catch (Exception ex)
                {
                    await _vocTypeRepository.RollbackTransaction(role);
                    throw ex;
                }
            }

            return result;
        }
    }
}
