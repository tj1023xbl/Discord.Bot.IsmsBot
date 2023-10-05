import { createTheme, Paper, Table, TableBody, TableCell, Input, TableContainer, TableHead, TableRow, FormControl, InputLabel, TextField, textFieldClasses, InputAdornment, typographyClasses, OutlinedInput, outlinedInputClasses } from "@mui/material"
import styleVariables from '../variables.module.scss'
import Saying from "../data/models/Saying"
import { ThemeProvider } from "@emotion/react";
import { useCallback, useState } from "react";


type Order = 'asc' | 'desc';
function descendingComparator<T>(a: T, b: T, orderBy: keyof T) {
    if (b[orderBy] < a[orderBy]) {
        return -1;
    }
    if (b[orderBy] > a[orderBy]) {
        return 1;
    }
    return 0;
}

function getComparator<Key extends keyof any>(
    order: Order,
    orderBy: Key,
): (
    a: { [key in Key]: number | string },
    b: { [key in Key]: number | string },
) => number {
    return order === 'desc'
        ? (a, b) => descendingComparator(a, b, orderBy)
        : (a, b) => -descendingComparator(a, b, orderBy);
}

const customTheme = createTheme({
    components: {
        MuiTypography: {
            styleOverrides: {
                root: {
                    [`&.${typographyClasses.root}`]: {
                        color: 'whitesmoke'
                    }
                },
            }
        },
        MuiTableRow: {
            styleOverrides: {
                head: {
                    backgroundColor: `#101010 !important`,
                    ['&:hover']: {
                        backgroundColor: `#101010 !important`,
                    }

                },
                root: {
                    backgroundColor: '#404040',
                    ['&:nth-of-type(odd)']: {
                        backgroundColor: '#303030',
                    },
                    ['&:hover']: {
                        backgroundColor: '#202020 !important',
                    }
                },
            },
        },
        MuiTableCell: {
            styleOverrides: {
                root: {
                    color: `whitesmoke`,
                    border: 'unset !important',
                },
                head: {
                    color: `${styleVariables.yellow_dark} !important`,
                }
            }
        },
        MuiFormLabel: {
            styleOverrides: {
                root: {
                    color: 'whitesmoke !important'
                }
            }
        },
        MuiOutlinedInput: {
            styleOverrides: {
                root: {
                    [`.${outlinedInputClasses.focused}`]: {
                        borderColor: `${styleVariables.yellow_dark}  !important`,
                        color: `${styleVariables.yellow_dark}  !important`,
                    }
                },
                input: {
                    color: 'white',
                },
            }
        },
        MuiInputBase: {
            styleOverrides: {
                root: {
                    [`&.${outlinedInputClasses.focused} .${outlinedInputClasses.notchedOutline}`]: {
                        borderColor: `${styleVariables.yellow_light}  !important`,
                        color: `${styleVariables.yellow_dark}  !important`,
                    }
                }
            }
        }
    }
})

console.log(`&.${outlinedInputClasses.focused} ${outlinedInputClasses.notchedOutline}`)

export const IsmTable = ({ sayings }: { sayings: Saying[] }) => {

    const [filter, setFilter] = useState('');

    const handleFilter = useCallback((saying: Saying) => {
        return saying.ismKey.includes(filter) ||
            saying.ismRecorder.includes(filter) ||
            saying.ismSaying.includes(filter)
    }, [filter])

    return (
        <>
            <ThemeProvider theme={customTheme}>
                <TextField onChange={(e) => { setFilter(e.target.value) }} value={filter} label='Filter' variant='outlined' InputProps={{ endAdornment: <InputAdornment sx={{ color: 'white' }} position="end">$</InputAdornment> }} />
                <TableContainer component={Paper}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>Saying</TableCell>
                                <TableCell>Key</TableCell>
                                <TableCell>DateTime</TableCell>
                                <TableCell>Recorder</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {sayings.filter(handleFilter).map((saying) => {
                                return (
                                    <TableRow key={saying.id}>
                                        <TableCell>{saying.ismSaying}</TableCell>
                                        <TableCell>{saying.ismKey}</TableCell>
                                        <TableCell>{saying.dateCreated.toString()}</TableCell>
                                        <TableCell>{saying.ismRecorder}</TableCell>
                                    </TableRow>
                                )
                            })}
                        </TableBody>
                    </Table>
                </TableContainer>
            </ThemeProvider>
        </>
    )
}
