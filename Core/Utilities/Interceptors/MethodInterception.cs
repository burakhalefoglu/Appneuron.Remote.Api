﻿using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace Core.Utilities.Interceptors
{
    public abstract class MethodInterceptionAttribute : MethodInterceptionBaseAttribute
    {
        protected virtual void OnBefore(IInvocation invocation)
        {
        }

        protected virtual void OnAfter(IInvocation invocation)
        {
        }

        protected virtual void OnException(IInvocation invocation, Exception e)
        {
        }

        protected virtual void OnSuccess(IInvocation invocation)
        {
        }

        public override void Intercept(IInvocation invocation)
        {
            var isSuccess = true;
            OnBefore(invocation);
            try
            {
                invocation.Proceed();
                var result = invocation.ReturnValue as Task;
                if (result != null)
                    result.Wait();
            }
            catch (Exception e)
            {
                isSuccess = false;
                OnException(invocation, e);
                throw;
            }
            finally
            {
                if (isSuccess) OnSuccess(invocation);
            }

            OnAfter(invocation);
        }
    }
    
    public abstract class MethodInterceptionAsyncAttribute : MethodInterceptionBaseAttribute
    {
        protected virtual async Task OnBefore(IInvocation invocation)
        {
        }

        protected virtual async Task  OnAfter(IInvocation invocation)
        {
        }

        protected virtual async Task  OnException(IInvocation invocation, Exception e)
        {
        }

        protected virtual async Task  OnSuccess(IInvocation invocation)
        {
        }

        public override async Task  InterceptAsync(IInvocation invocation)
        {
            var isSuccess = true;
            OnBefore(invocation);
            try
            {
                invocation.Proceed();
                var result = invocation.ReturnValue as Task;
                if (result != null)
                    result.Wait();
            }
            catch (Exception e)
            {
                isSuccess = false;
                OnException(invocation, e);
                throw;
            }
            finally
            {
                if (isSuccess) OnSuccess(invocation);
            }

            OnAfter(invocation);
        }
    }
}