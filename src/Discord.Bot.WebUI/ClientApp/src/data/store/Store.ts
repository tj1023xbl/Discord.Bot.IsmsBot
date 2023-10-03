import { Action, configureStore, ThunkAction } from '@reduxjs/toolkit';
import { IsmSlice } from './IsmSlice';

export const store = configureStore({
    reducer: {
        Isms: IsmSlice.reducer
    }
})

/* EXPORT DATA TYPES */
export const AppDispatch = store.dispatch;
export type RootState = ReturnType<typeof store.getState>
export type AppThunk<ReturnType = void> = ThunkAction<
    ReturnType,
    RootState,
    unknown,
    Action<string>
>
