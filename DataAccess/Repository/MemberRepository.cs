using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class MemberRepository : IMemberRepository
    {
        public IEnumerable<Member> GetAll() => MemberDAO.Instance.GetAll();

        public Member? GetByEmail(string email) => MemberDAO.Instance.GetByEmail(email);

        public Member? GetById(int id) => MemberDAO.Instance.GetById(id);

        public void Insert(Member member) => MemberDAO.Instance.Add(member);

        public void Remove(int id) => MemberDAO.Instance.Remove(id);

        public void Update(Member member) => MemberDAO.Instance.Update(member);

        public Member Login(string email, string password) => MemberDAO.Instance.Login(email, password);
    }
}
