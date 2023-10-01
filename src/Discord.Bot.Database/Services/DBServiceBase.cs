using Discord.Bot.Database;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Discord.Bot.IsmsBot.WebUI.Services
{
    /// <summary>
    /// This service base is meant to be extended by a service which makes use
    /// of the EF Core database. EF Core is not thread safe, and the same instance
    /// of EF Core cannot be accessed by multiple threads concurrently. 
    /// </summary>
    public abstract class DBServiceBase
    {
        private readonly AppDBContext _db;
        private readonly SemaphoreSlim _ss;

        protected DBServiceBase(AppDBContext db, SemaphoreSlim ss)
        {
            _db = db;
            _ss = ss;
        }


        /// <summary>
        /// Add a semaphore to make sure multiple threads don't access the databse at the same time. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        protected async Task<T> ShieldDb<T>(Func<AppDBContext, Task<T>> func)
        {
            try
            {
                await _ss.WaitAsync();
                return await func.Invoke(_db);
            }
            catch (Exception ex)
            {
                Log.ForContext("Function", func, true)
                   .Error(ex, "Something went wrong while accessing the DB");
                throw;
            }
            finally { _ss.Release(); }
        }

        /// <summary>
        /// Add a semaphore to make sure multiple threads don't access the databse at the same time. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        protected async Task<T> ShieldDb<T>(Func<AppDBContext, T> func)
        {
            try
            {
                await _ss.WaitAsync();
                return func.Invoke(_db);
            }
            catch (Exception ex)
            {
                Log.ForContext("Function", func, true)
                   .Error(ex, "Something went wrong while accessing the DB");
                throw;
            }
            finally { _ss.Release(); }
        }

        /// <summary>
        /// Add a semaphore to make sure multiple threads don't access the databse at the same time. 
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        protected async Task ShieldDb(Action<AppDBContext> func)
        {
            try
            {
                await _ss.WaitAsync();
                func.Invoke(_db);
            }
            catch (Exception ex)
            {
                Log.ForContext("Function", func, true)
                   .Error(ex, "Something went wrong while accessing the DB");
                throw;
            }
            finally { _ss.Release(); }
        }
    }
}
