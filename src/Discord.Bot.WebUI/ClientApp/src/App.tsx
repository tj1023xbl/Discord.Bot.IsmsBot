import { useEffect } from 'react';
import styles from './App.module.scss';
import { getAllGuildsAsyncThunk } from './data/store/IsmSlice';
import { AppDispatch } from './data/store/Store';

/**
 * Homepage
 * @returns
 */
export default function App() { 

    useEffect(() => {
        AppDispatch(getAllGuildsAsyncThunk());
    }, [])

    return (
        <div className={styles.container}>
            <h1 className={styles.intro}>Isms Bot</h1>
            <section>
                {/*<PillSection />*/}
            </section>
        </div>
    )
}

