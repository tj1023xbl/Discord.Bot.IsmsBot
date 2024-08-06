import { Action, configureStore, ThunkAction } from '@reduxjs/toolkit';
import { GuildSlice } from './GuildSlice';
import { IsmSlice } from './IsmSlice';
import { AuthSlice } from './AuthSlice';
import { LoadingSlice } from './LoadingSlice';

export const store = configureStore({
    reducer: {
        Isms: IsmSlice.reducer,
        Guilds: GuildSlice.reducer,
        AuthState: AuthSlice.reducer,
        LoadingState: LoadingSlice.reducer
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

export type Status = 'loading' | 'idle' | 'complete' | 'failed';
