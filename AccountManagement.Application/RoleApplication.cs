using _01_Framework.Application;
using AccountManagement.Application.Contracts.Role;
using AccountManagement.Domain.RoleAgg;

namespace AccountManagement.Application
{
    public class RoleApplication : IRoleApplication
    {
        private readonly IRoleRepository _roleRepository;

        public RoleApplication(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public OperationResult Create(CreateRole command)
        {
            OperationResult operation = new();
            if (_roleRepository.Exsists(x => x.Name == command.Name))
                operation.Failed(ApplicationMessages.DuplicatedRecored);

            var role = new Role(command.Name);

            _roleRepository.Create(role);
            _roleRepository.SaveChanges();
            return operation.Succeded();
        }

        public OperationResult Edit(EditRole command)
        {
            OperationResult operation = new();

            var role = _roleRepository.Get(command.Id);
            if (role == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (_roleRepository.Exsists(x => x.Name == command.Name && x.Id != command.Id))
                operation.Failed(ApplicationMessages.DuplicatedRecored);

            role.Edit(command.Name);

            role.Edit(command.Name);
            _roleRepository.SaveChanges();
            return operation.Succeded();
        }

        public EditRole GetDetails(long id)
        {
            return _roleRepository.GetDetails(id);
        }

        public List<RoleViewModel> List()
        {
            return _roleRepository.List();

        }
    }
}
