/* tslint:disable */
/* eslint-disable */
//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.13.2.0 (NJsonSchema v10.5.2.0 (Newtonsoft.Json v12.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------
// ReSharper disable InconsistentNaming

import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable()
export class AnimalService {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    }

    getOwnedAnimals(userId: string | null, pageSize: number | undefined, pageIndex: number | undefined): Observable<PagedListOfOwnedAnimalDto> {
        let url_ = this.baseUrl + "/api/animals/list/{userId}?";
        if (userId === undefined || userId === null)
            throw new Error("The parameter 'userId' must be defined.");
        url_ = url_.replace("{userId}", encodeURIComponent("" + userId));
        if (pageSize === null)
            throw new Error("The parameter 'pageSize' cannot be null.");
        else if (pageSize !== undefined)
            url_ += "PageSize=" + encodeURIComponent("" + pageSize) + "&";
        if (pageIndex === null)
            throw new Error("The parameter 'pageIndex' cannot be null.");
        else if (pageIndex !== undefined)
            url_ += "PageIndex=" + encodeURIComponent("" + pageIndex) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetOwnedAnimals(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetOwnedAnimals(<any>response_);
                } catch (e) {
                    return <Observable<PagedListOfOwnedAnimalDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedListOfOwnedAnimalDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetOwnedAnimals(response: HttpResponseBase): Observable<PagedListOfOwnedAnimalDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            result200 = _responseText === "" ? null : <PagedListOfOwnedAnimalDto>JSON.parse(_responseText, this.jsonParseReviver);
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedListOfOwnedAnimalDto>(<any>null);
    }

    createAnimal(userId: string | null, name: string | null | undefined, dateOfBirth: Date | undefined, sex: string | null | undefined, speciesId: string | null | undefined, photo: FileParameter | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/animals/{userId}";
        if (userId === undefined || userId === null)
            throw new Error("The parameter 'userId' must be defined.");
        url_ = url_.replace("{userId}", encodeURIComponent("" + userId));
        url_ = url_.replace(/[?&]$/, "");

        const content_ = new FormData();
        if (name !== null && name !== undefined)
            content_.append("Name", name.toString());
        if (dateOfBirth === null || dateOfBirth === undefined)
            throw new Error("The parameter 'dateOfBirth' cannot be null.");
        else
            content_.append("DateOfBirth", dateOfBirth.toJSON());
        if (sex !== null && sex !== undefined)
            content_.append("Sex", sex.toString());
        if (speciesId !== null && speciesId !== undefined)
            content_.append("SpeciesId", speciesId.toString());
        if (photo !== null && photo !== undefined)
            content_.append("Photo", photo.data, photo.fileName ? photo.fileName : "Photo");

        let options_ : any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processCreateAnimal(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processCreateAnimal(<any>response_);
                } catch (e) {
                    return <Observable<void>><any>_observableThrow(e);
                }
            } else
                return <Observable<void>><any>_observableThrow(response_);
        }));
    }

    protected processCreateAnimal(response: HttpResponseBase): Observable<void> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return _observableOf<void>(<any>null);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<void>(<any>null);
    }

    getAnimal(animalId: string | null): Observable<AnimalDto> {
        let url_ = this.baseUrl + "/api/animals/{animalId}";
        if (animalId === undefined || animalId === null)
            throw new Error("The parameter 'animalId' must be defined.");
        url_ = url_.replace("{animalId}", encodeURIComponent("" + animalId));
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetAnimal(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetAnimal(<any>response_);
                } catch (e) {
                    return <Observable<AnimalDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<AnimalDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAnimal(response: HttpResponseBase): Observable<AnimalDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            result200 = _responseText === "" ? null : <AnimalDto>JSON.parse(_responseText, this.jsonParseReviver);
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<AnimalDto>(<any>null);
    }
}

@Injectable()
export class SpeciesService {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    }

    getAnimalSpecies(): Observable<AnimalSpeciesDto[]> {
        let url_ = this.baseUrl + "/api/species";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetAnimalSpecies(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetAnimalSpecies(<any>response_);
                } catch (e) {
                    return <Observable<AnimalSpeciesDto[]>><any>_observableThrow(e);
                }
            } else
                return <Observable<AnimalSpeciesDto[]>><any>_observableThrow(response_);
        }));
    }

    protected processGetAnimalSpecies(response: HttpResponseBase): Observable<AnimalSpeciesDto[]> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            result200 = _responseText === "" ? null : <AnimalSpeciesDto[]>JSON.parse(_responseText, this.jsonParseReviver);
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<AnimalSpeciesDto[]>(<any>null);
    }
}

export interface PagedListOfOwnedAnimalDto {
    pageIndex?: number;
    pageSize?: number;
    totalCount?: number;
    items?: OwnedAnimalDto[] | undefined;
}

export interface OwnedAnimalDto {
    id?: string;
    name?: string | undefined;
    dateOfBirth?: Date;
    age?: string | undefined;
    sex?: string | undefined;
    speciesName?: string | undefined;
    subSpeciesName?: string | undefined;
    photoUrl?: string | undefined;
}

export interface AnimalDto {
    id?: string;
    name?: string | undefined;
    dateOfBirth?: Date;
    age?: string | undefined;
    sex?: string | undefined;
    speciesId?: string;
    subSpeciesName?: string | undefined;
    photoUrl?: string | undefined;
}

export interface AnimalSpeciesDto {
    id?: string;
    name?: string | undefined;
}

export interface FileParameter {
    data: any;
    fileName: string;
}

export class ApiException extends Error {
    message: string;
    status: number;
    response: string;
    headers: { [key: string]: any; };
    result: any;

    constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
    }

    protected isApiException = true;

    static isApiException(obj: any): obj is ApiException {
        return obj.isApiException === true;
    }
}

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): Observable<any> {
    if (result !== null && result !== undefined)
        return _observableThrow(result);
    else
        return _observableThrow(new ApiException(message, status, response, headers, null));
}

function blobToText(blob: any): Observable<string> {
    return new Observable<string>((observer: any) => {
        if (!blob) {
            observer.next("");
            observer.complete();
        } else {
            let reader = new FileReader();
            reader.onload = event => {
                observer.next((<any>event.target).result);
                observer.complete();
            };
            reader.readAsText(blob);
        }
    });
}