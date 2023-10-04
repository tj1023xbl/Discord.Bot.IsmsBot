import { Action, configureStore, ThunkAction } from '@reduxjs/toolkit';
import { GuildSlice, IsmSlice } from './IsmSlice';

export const store = configureStore({
    reducer: {
        Isms: IsmSlice.reducer,
        Guilds: GuildSlice.reducer
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
